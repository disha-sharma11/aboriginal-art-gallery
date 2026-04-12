import { useEffect, useMemo, useState } from 'react';
import PageLayout from '../Components/PageLayout';
import { getTribes } from '../services/api';
import './TribesPage.css';

function TribesPage() {
  const [tribes, setTribes] = useState([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState('');
  const [searchTerm, setSearchTerm] = useState('');
  const [selectedState, setSelectedState] = useState('');

  useEffect(() => {
    async function loadTribes() {
      try {
        const data = await getTribes();
        setTribes(data);
      } catch (err) {
        setError(err.message);
      } finally {
        setLoading(false);
      }
    }

    loadTribes();
  }, []);

  const stateOptions = useMemo(() => {
    return [...new Set(tribes.map((tribe) => tribe.stateOrTerritory).filter(Boolean))];
  }, [tribes]);

  const filteredTribes = useMemo(() => {
    return tribes.filter((tribe) => {
      const matchesSearch =
        tribe.name.toLowerCase().includes(searchTerm.toLowerCase()) ||
        tribe.originRegionName.toLowerCase().includes(searchTerm.toLowerCase());

      const matchesState = selectedState ? tribe.stateOrTerritory === selectedState : true;

      return matchesSearch && matchesState;
    });
  }, [tribes, searchTerm, selectedState]);

  return (
    <PageLayout title="Tribes">
      {loading && <p className="tribes-page__status">Loading tribes...</p>}
      {error && <p className="tribes-page__status">{error}</p>}

      <div className="tribes-filters">
        <input
          type="text"
          placeholder="Search by name or region"
          value={searchTerm}
          onChange={(event) => setSearchTerm(event.target.value)}
          className="tribes-filters__input"
        />

        <select
          value={selectedState}
          onChange={(event) => setSelectedState(event.target.value)}
          className="tribes-filters__select"
        >
          <option value="">All States/Territories</option>
          {stateOptions.map((state) => (
            <option key={state} value={state}>
              {state}
            </option>
          ))}
        </select>
      </div>

      <div className="tribes-grid">
        {filteredTribes.map((tribe) => (
          <div key={tribe.id} className="tribe-card">
            <h3 className="tribe-card__title">{tribe.name}</h3>
            <div className="tribe-card__details">
              <p><span>Name:</span> {tribe.name}</p>
              <p><span>State/Territory:</span> {tribe.stateOrTerritory}</p>
              <p><span>Origin Region:</span> {tribe.originRegionName}</p>
              <p><span>Latitude:</span> {tribe.latitude ?? 'N/A'}</p>
              <p><span>Longitude:</span> {tribe.longitude ?? 'N/A'}</p>
            </div>
            <p className="tribe-card__description">{tribe.description}</p>
          </div>
        ))}
      </div>
    </PageLayout>
  );
}

export default TribesPage;
