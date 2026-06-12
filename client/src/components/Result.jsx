function Result({ result, onDismiss }) {
  if (result === null) return null

  return (
    <div className={`result-overlay ${result ? 'is-hotdog' : 'not-hotdog'}`} onClick={onDismiss}>
      <div className="result-content">
        <div className="result-icon">{result ? '🌭' : '🚫'}</div>
        <h2 className="result-label">{result ? 'Hotdog!' : 'Not Hotdog!'}</h2>
        <p className="result-dismiss">Tap to continue</p>
      </div>
    </div>
  )
}

export default Result
