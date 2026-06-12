using Microsoft.ML.OnnxRuntime;
using Microsoft.ML.OnnxRuntime.Tensors;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;

public class HotdogClassifierService
{
    // Holds the loaded ONNX model. Creating a session is expensive (loads weights into memory),
    // so we keep one alive for the lifetime of the app rather than recreating it per request.
    private readonly InferenceSession _session;

    public HotdogClassifierService()
    {
        // AppContext.BaseDirectory is the folder where the compiled .dll lives at runtime —
        // NOT the source folder. The model files are copied there by the .csproj CopyToOutputDirectory rule.
        var modelPath = Path.Combine(AppContext.BaseDirectory, "Models", "hot_dog_not_hot_dog_resnet.onnx");

        // ONNX Runtime automatically picks up the .onnx.data file from the same directory.
        _session = new InferenceSession(modelPath);

        // Log node names on startup so we can confirm what the model expects.
        // The input name is used to build the named tensor below.
        System.Console.WriteLine("Input nodes: " + string.Join(", ", _session.InputMetadata.Keys));
        System.Console.WriteLine("Output nodes: " + string.Join(", ", _session.OutputMetadata.Keys));
    }

    public bool Classify(IFormFile file)
    {
        using var stream = file.OpenReadStream();

        // Decode the uploaded image from the raw stream.
        using var image = Image.Load<Rgb24>(stream);

        // ResNet was trained on 224x224 images — any other size produces wrong results.
        image.Mutate(x => x.Resize(224, 224));

        // Flat array representing shape [1, 3, 224, 224] — batch, channels, height, width.
        // This layout is called NCHW and is what PyTorch-exported ResNet models expect.
        var tensorData = new float[1 * 3 * 224 * 224];

        for (int y = 0; y < 224; y++)
        {
            for (int x = 0; x < 224; x++)
            {
                var pixel = image[x, y];

                // Images store pixels interleaved (RGBRGBRGB...) but NCHW needs all of one
                // channel together. The channel offset (0/1/2 * 224 * 224) separates them.
                //
                // Normalization formula: (value / 255 - mean) / std
                // Scales 0-255 → 0-1, then shifts to match the ImageNet distribution
                // that ResNet was originally trained on. These constants are fixed for all
                // ResNet models trained on ImageNet.
                tensorData[0 * 224 * 224 + y * 224 + x] = (pixel.R / 255f - 0.485f) / 0.229f; // R
                tensorData[1 * 224 * 224 + y * 224 + x] = (pixel.G / 255f - 0.456f) / 0.224f; // G
                tensorData[2 * 224 * 224 + y * 224 + x] = (pixel.B / 255f - 0.406f) / 0.225f; // B
            }
        }

        // Wrap the flat array in a tensor, telling ONNX Runtime its shape.
        var tensor = new DenseTensor<float>(tensorData, [1, 3, 224, 224]);

        // Pair the tensor with the model's expected input node name.
        var inputName = _session.InputMetadata.Keys.First();
        var inputs = new[] { NamedOnnxValue.CreateFromTensor(inputName, tensor) };

        using var results = _session.Run(inputs);

        // Output is 2 raw scores (logits): [not_hotdog, hotdog].
        // No need for softmax — comparing the raw values directly is sufficient.
        // If results are always wrong, the class order may be flipped: swap to output[0] > output[1].
        var output = results[0].AsEnumerable<float>().ToArray();
        return output[0] > output[1];
    }
}