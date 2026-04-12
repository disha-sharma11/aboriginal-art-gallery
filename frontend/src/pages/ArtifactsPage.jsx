import { useEffect, useMemo, useState } from 'react';
import { Link } from 'react-router-dom';
import PageLayout from '../Components/PageLayout';
import { getArtifacts } from '../services/api';
import './ArtifactsPage.css';

function ArtifactsPage() {
  const [artifacts, setArtifacts] = useState([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState('');
  const [searchTerm, setSearchTerm] = useState('');
  const [selectedTribe, setSelectedTribe] = useState('');
  const [selectedType, setSelectedType] = useState('');

  useEffect(() => {
    async function loadArtifacts() {
      try {
        const data = await getArtifacts();
        setArtifacts(data);
      } catch (err) {
        setError(err.message);
      } finally {
        setLoading(false);
      }
    }

    loadArtifacts();
  }, []);

  const tribeOptions = useMemo(() => {
    return [...new Set(artifacts.map((artifact) => artifact.tribeName).filter(Boolean))];
  }, [artifacts]);

  const typeOptions = useMemo(() => {
    return [...new Set(artifacts.map((artifact) => artifact.artType).filter(Boolean))];
  }, [artifacts]);

  const filteredArtifacts = useMemo(() => {
    return artifacts.filter((artifact) => {
      const matchesSearch =
        artifact.title.toLowerCase().includes(searchTerm.toLowerCase()) ||
        artifact.artistName.toLowerCase().includes(searchTerm.toLowerCase());

      const matchesTribe = selectedTribe ? artifact.tribeName === selectedTribe : true;
      const matchesType = selectedType ? artifact.artType === selectedType : true;

      return matchesSearch && matchesTribe && matchesType;
    });
  }, [artifacts, searchTerm, selectedTribe, selectedType]);

  return (
    <PageLayout title="Artifacts">
      {loading && <p className="artifacts-page__status">Loading artifacts...</p>}
      {error && <p className="artifacts-page__status">{error}</p>}

      <div className="artifacts-filters">
        <input
          type="text"
          placeholder="Search by title or artist"
          value={searchTerm}
          onChange={(event) => setSearchTerm(event.target.value)}
          className="artifacts-filters__input"
        />

        <select
          value={selectedTribe}
          onChange={(event) => setSelectedTribe(event.target.value)}
          className="artifacts-filters__select"
        >
          <option value="">All Tribes</option>
          {tribeOptions.map((tribe) => (
            <option key={tribe} value={tribe}>
              {tribe}
            </option>
          ))}
        </select>

        <select
          value={selectedType}
          onChange={(event) => setSelectedType(event.target.value)}
          className="artifacts-filters__select"
        >
          <option value="">All Types</option>
          {typeOptions.map((type) => (
            <option key={type} value={type}>
              {type}
            </option>
          ))}
        </select>
      </div>

      <div className="artifacts-grid">
        {filteredArtifacts.map((artifact) => (
          <Link
            key={artifact.id}
            to={`/artifacts/${artifact.id}`}
            className="artifact-card"
          >
            {artifact.imageUrl ? (
              <img
                src={artifact.imageUrl}
                alt={artifact.title}
                className="artifact-card__image"
              />
            ) : (
              <div className="artifact-card__image artifact-card__image--placeholder">
                No Image
              </div>
            )}

            <h3 className="artifact-card__title">{artifact.title}</h3>

            <div className="artifact-card__details">
              <p><span>Name:</span> {artifact.title}</p>
              <p><span>Type:</span> {artifact.artType}</p>
              <p><span>Artist:</span> {artifact.artistName}</p>
              <p><span>Tribe:</span> {artifact.tribeName}</p>
              <p><span>Exhibition:</span> {artifact.exhibitionName ?? 'N/A'}</p>
              <p><span>Origin:</span> {artifact.originPlaceName}</p>
            </div>
          </Link>
        ))}
      </div>
    </PageLayout>
  );
}

export default ArtifactsPage;
