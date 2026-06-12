# SeeFood

A recreation of the SeeFood app from Silicon Valley. Upload a photo of any food and the app will tell you whether it is a hot dog or not a hot dog.

## How it works

The classifier is a ResNet model with its final classification layer fine-tuned on the [Hot Dog / Not Hot Dog dataset](https://www.kaggle.com/datasets/dansbecker/hot-dog-not-hot-dog) from Kaggle. The base ResNet weights come from ImageNet pretraining; only the head was retrained for the two-class task. The model is exported as ONNX and runs server-side via ONNX Runtime.

## Stack

- **Client** — React + Vite
- **Server** — ASP.NET Core (.NET 10), ONNX Runtime, SkiaSharp

## Installation

### Prerequisites

- [Node.js](https://nodejs.org) (v18+)
- [.NET 10 SDK](https://dotnet.microsoft.com/download)

### Server

```bash
cd server/HotdogApi
dotnet run
```

The API will start on `http://localhost:5245`.

The ONNX model files (`hot_dog_not_hot_dog_resnet.onnx` and `.onnx.data`) must be placed in `server/HotdogApi/Models/` before running.

### Client

```bash
cd client
npm install
npm run dev
```

The app will be available at `http://localhost:5173`.

To point the client at a different backend, create a `.env` file in the `client/` directory:

```
VITE_API_URL=http://localhost:5245
```

## Credits

Hot dog icon by [Adib Sulthon](https://www.flaticon.com/authors/adib-sulthon) via Flaticon.
