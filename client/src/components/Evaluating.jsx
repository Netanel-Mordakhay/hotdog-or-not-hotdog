function Evaluating({ loading }) {
  if (!loading) return null;

  return (
    <div className="overlay">
      <div className="loader" />
      <span className="loading-text">Evaluating...</span>
    </div>
  );
}

export default Evaluating;
