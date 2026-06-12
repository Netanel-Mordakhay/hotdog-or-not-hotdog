import axios from "axios";

const client = axios.create({
  baseURL: import.meta.env.VITE_API_URL,
});

export async function classifyImage(imageData) {
  const formData = new FormData();
  formData.append("image", imageData);

  try {
    const response = await client.post("/api/hotdog", formData);
    return response.data;
  } catch (error) {
    console.error("Error classifying image:", error);
    throw error;
  }
}
