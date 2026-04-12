import { useEffect, useMemo, useState } from 'react';
import PageLayout from '../Components/PageLayout';
import { getExhibitions } from '../services/api';
import './ExhibitionsPage.css';

function ExhibitionsPage() {
  const [exhibitions, setExhibitions] = useState([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState('');
  const [searchTerm, setSearchTerm] = useState('');

  useEffect(() => {
    async function loadExhibitions() {
      try {
        const data = await getExhibitions();
        setExhibitions(data);
      } catch (err) {
        setError(err.message);
      } finally {
        setLoading(false);
      }
    }

    loadExhibitions();
  }, []);

  const filteredExhibitions = useMemo(() => {
    return exhibitions.filter((exhibition) => {
      const search = searchTerm.toLowerCase();

      return (
        exhibition.name.toLowerCase().includes(search) ||
        exhibition.location.toLowerCase().includes(search)
      );
    });
  }, [exhibitions, searchTerm]);

  function formatDate(value) {
    return new Date(value).toLocaleDateString();
  }

  return (
    <PageLayout title="Exhibitions">
      {loading && <p className="exhibitions-page__status">Loading exhibitions...</p>}
      {error && <p className="exhibitions-page__status">{error}</p>}

      <div className="exhibitions-filters">
        <input
          type="text"
          placeholder="Search by name or location"
          value={searchTerm}
          onChange={(event) => setSearchTerm(event.target.value)}
          className="exhibitions-filters__input"
        />
      </div>

      <div className="exhibitions-grid">
        {filteredExhibitions.map((exhibition) => (
          <div key={exhibition.id} className="exhibition-card">
            <h3 className="exhibition-card__title">{exhibition.name}</h3>
            <div className="exhibition-card__details">
              <p><span>Location:</span> {exhibition.location}</p>
              <p><span>Start:</span> {formatDate(exhibition.startDate)}</p>
              <p><span>End:</span> {formatDate(exhibition.endDate)}</p>
            </div>
            <p className="exhibition-card__description">{exhibition.description}</p>
          </div>
        ))}
      </div>
    </PageLayout>
  );
}

export default ExhibitionsPage;
