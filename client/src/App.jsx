import { useState } from "react";
import { classifyImage } from "./api/hotdog";
import ImageUpload from "./components/ImageUpload";
import Result from "./components/Result";
import Evaluating from "./components/Evaluating";
import "./App.css";
import logoImage from "./assets/hot-dog.png";
import winSound from "./assets/sounds/win.wav";
import loseSound from "./assets/sounds/lose.mp3";

function App() {
  const [file, setFile] = useState(null);
  const [preview, setPreview] = useState(null);
  const [result, setResult] = useState(null);
  const [loading, setLoading] = useState(false);
  const [error, setError] = useState(null);

  function handleFileChange(selectedFile) {
    setFile(selectedFile);
    setPreview(URL.createObjectURL(selectedFile));
    setResult(null);
    setError(null);
  }

  async function handleSubmit() {
    if (!file) return;
    setLoading(true);
    setError(null);

    try {
      const [data] = await Promise.all([
        classifyImage(file),
        new Promise((resolve) => setTimeout(resolve, 2000)),
      ]);
      setResult(data.isHotdog);
      new Audio(data.isHotdog ? winSound : loseSound).play();
    } catch (err) {
      setError("Something went wrong. Is the server running?");
    } finally {
      setLoading(false);
    }
  }

  return (
    <div className="app">
      <Evaluating loading={loading} />

      <header className="app-header">
        <div className="logo">
          <img
            src={logoImage}
            alt="Logo"
            style={{ width: "50px", height: "50px" }}
          />
          <span className="logo-text">SeeFood</span>
        </div>
      </header>

      <div className="hero">
        <h1>What's on your plate?</h1>
        <p>Point your camera at any dish for instant identification</p>
      </div>

      <Result result={result} onDismiss={() => setResult(null)} />

      <ImageUpload onFileChange={handleFileChange} preview={preview} />

      <div className="actions">
        <button
          className="btn-primary"
          onClick={handleSubmit}
          disabled={!file || loading}
        >
          <svg
            viewBox="0 0 24 24"
            fill="none"
            stroke="currentColor"
            strokeWidth="2"
            width="20"
            height="20"
          >
            <path d="M23 19a2 2 0 0 1-2 2H3a2 2 0 0 1-2-2V8a2 2 0 0 1 2-2h4l2-3h6l2 3h4a2 2 0 0 1 2 2z" />
            <circle cx="12" cy="13" r="4" />
          </svg>
          Scan Food
        </button>
        {error && <p className="error">{error}</p>}
      </div>
      <div className="credit">
        yes, it's the app from Silicon Valley ·{" "}
        <a href="https://netanelmordakhay.com" target="_blank" rel="noreferrer">
          Netanel Mordakhay
        </a>
        <br />
        ResNet with a fine-tuned head on the{" "}
        <a
          href="https://www.kaggle.com/datasets/dansbecker/hot-dog-not-hot-dog"
          target="_blank"
          rel="noreferrer"
        >
          Kaggle hot dog dataset
        </a>
        <br />
        icon by{" "}
        <a
          href="https://www.flaticon.com/authors/adib-sulthon"
          target="_blank"
          rel="noreferrer"
        >
          Adib Sulthon
        </a>{" "}
        via Flaticon
      </div>
    </div>
  );
}

export default App;
