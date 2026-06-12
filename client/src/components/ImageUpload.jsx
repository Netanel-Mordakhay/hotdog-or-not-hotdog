function ImageUpload({ onFileChange, preview }) {
  function handleChange(e) {
    const file = e.target.files[0]
    if (file) onFileChange(file)
  }

  return (
    <label className="image-area">
      <input type="file" accept="image/*" onChange={handleChange} hidden />

      {preview ? (
        <img src={preview} alt="Preview" className="preview-img" />
      ) : (
        <div className="image-placeholder">
          <div className="viewfinder">
            <span className="corner tl" />
            <span className="corner tr" />
            <span className="corner bl" />
            <span className="corner br" />
            <svg className="camera-svg" viewBox="0 0 24 24" fill="none" stroke="currentColor" strokeWidth="1.5" width="48" height="48">
              <path d="M23 19a2 2 0 0 1-2 2H3a2 2 0 0 1-2-2V8a2 2 0 0 1 2-2h4l2-3h6l2 3h4a2 2 0 0 1 2 2z" />
              <circle cx="12" cy="13" r="4" />
            </svg>
          </div>
          <p className="upload-hint">Drop a photo or tap to upload</p>
          <p className="upload-sub">Any food, any angle</p>
        </div>
      )}
    </label>
  )
}

export default ImageUpload
