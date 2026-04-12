import { useCallback, useEffect, useState } from "react";
import { useParams } from "react-router-dom";
import PageLayout from "../Components/PageLayout";
import {
  getArtifactById,
  getCommentsByArtifactId,
  createComment,
} from "../services/api";
import "./ArtifactDetailsPage.css";

function ArtifactDetailsPage() {
  const { id } = useParams();
  const [artifact, setArtifact] = useState(null);
  const [comments, setComments] = useState([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState("");
  const [formError, setFormError] = useState("");
  const [submitting, setSubmitting] = useState(false);

  const [formData, setFormData] = useState({
    visitorName: "",
    visitorEmail: "",
    content: "",
    rating: "",
  });

  const loadData = useCallback(async () => {
    try {
      const artifactData = await getArtifactById(id);
      const commentsData = await getCommentsByArtifactId(id);
      setArtifact(artifactData);
      setComments(commentsData);
    } catch (err) {
      setError(err.message);
    } finally {
      setLoading(false);
    }
  }, [id]);

  useEffect(() => {
    loadData();
  }, [loadData]);

  function handleChange(event) {
    const { name, value } = event.target;
    setFormData((prev) => ({
      ...prev,
      [name]: value,
    }));
  }

  async function handleSubmit(event) {
    event.preventDefault();
    setFormError("");
    setSubmitting(true);

    try {
      await createComment({
        visitorName: formData.visitorName,
        visitorEmail: formData.visitorEmail || null,
        content: formData.content,
        rating: formData.rating ? Number(formData.rating) : null,
        artifactId: Number(id),
      });

      setFormData({
        visitorName: "",
        visitorEmail: "",
        content: "",
        rating: "",
      });

      const updatedComments = await getCommentsByArtifactId(id);
      setComments(updatedComments);
    } catch (err) {
      setFormError(err.message);
    } finally {
      setSubmitting(false);
    }
  }

  return (
    <PageLayout title="Artifact Details">
      {loading && (
        <p className="artifact-details__status">Loading artifact...</p>
      )}
      {error && <p className="artifact-details__status">{error}</p>}

      {artifact && (
        <div className="artifact-details">
          <div className="artifact-details__card">
            <h2 className="artifact-details__name">{artifact.title}</h2>
            {artifact.imageUrl ? (
              <img
                src={artifact.imageUrl}
                alt={artifact.title}
                className="artifact-details__image"
              />
            ) : null}

            <p className="artifact-details__description">
              {artifact.description}
            </p>

            <div className="artifact-details__meta">
              <p>
                <span>Name:</span> {artifact.title}
              </p>
              <p>
                <span>Artist:</span> {artifact.artistName}
              </p>
              <p>
                <span>Tribe:</span> {artifact.tribeName}
              </p>
              <p>
                <span>Exhibition:</span> {artifact.exhibitionName ?? "N/A"}
              </p>
              <p>
                <span>Type:</span> {artifact.artType}
              </p>
              <p>
                <span>Style:</span> {artifact.artStyle}
              </p>
              <p>
                <span>Era:</span> {artifact.era}
              </p>
              <p>
                <span>Material:</span> {artifact.material}
              </p>
              <p>
                <span>Origin:</span> {artifact.originPlaceName}
              </p>
            </div>
          </div>

          <div className="artifact-comments">
            <h3>Comments</h3>
            {comments.length === 0 && <p>No comments yet.</p>}
            {comments.map((comment) => (
              <div key={comment.id} className="artifact-comment">
                <p>
                  <strong>Name:</strong> {comment.visitorName}
                </p>
                <p>
                  <strong>Comment:</strong> {comment.content}
                </p>
                <p>
                  <strong>Rating:</strong> {comment.rating ?? "N/A"}
                </p>
              </div>
            ))}
          </div>

          <div className="artifact-form">
            <h3>Add a Comment</h3>

            <form onSubmit={handleSubmit} className="artifact-form__form">
              <label>
                Name
                <input
                  type="text"
                  name="visitorName"
                  value={formData.visitorName}
                  onChange={handleChange}
                  required
                />
              </label>

              <label>
                Email
                <input
                  type="email"
                  name="visitorEmail"
                  value={formData.visitorEmail}
                  onChange={handleChange}
                />
              </label>

              <label>
                Comment
                <textarea
                  name="content"
                  value={formData.content}
                  onChange={handleChange}
                  rows="4"
                  required
                />
              </label>

              <label>
                Rating
                <input
                  type="number"
                  name="rating"
                  min="1"
                  max="5"
                  value={formData.rating}
                  onChange={handleChange}
                />
              </label>

              {formError && <p className="artifact-form__error">{formError}</p>}

              <button type="submit" disabled={submitting}>
                {submitting ? "Submitting..." : "Submit Comment"}
              </button>
            </form>
          </div>
        </div>
      )}
    </PageLayout>
  );
}

export default ArtifactDetailsPage;
