import { useEffect, useMemo, useState } from 'react';
import PageLayout from '../Components/PageLayout';
import { getArtists } from '../services/api';
import './ArtistsPage.css';

function ArtistsPage() {
  const [artists, setArtists] = useState([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState('');
  const [searchTerm, setSearchTerm] = useState('');
  const [selectedTribe, setSelectedTribe] = useState('');

  useEffect(() => {
    async function loadArtists() {
      try {
        const data = await getArtists();
        setArtists(data);
      } catch (err) {
        setError(err.message);
      } finally {
        setLoading(false);
      }
    }

    loadArtists();
  }, []);

  const tribeOptions = useMemo(() => {
    return [...new Set(artists.map((artist) => artist.tribeName).filter(Boolean))];
  }, [artists]);

  const filteredArtists = useMemo(() => {
    return artists.filter((artist) => {
      const matchesSearch =
        artist.fullName.toLowerCase().includes(searchTerm.toLowerCase()) ||
        artist.region.toLowerCase().includes(searchTerm.toLowerCase());

      const matchesTribe = selectedTribe ? artist.tribeName === selectedTribe : true;

      return matchesSearch && matchesTribe;
    });
  }, [artists, searchTerm, selectedTribe]);

  return (
    <PageLayout title="Artists">
      {loading && <p className="artists-page__status">Loading artists...</p>}
      {error && <p className="artists-page__status">{error}</p>}

      <div className="artists-filters">
        <input
          type="text"
          placeholder="Search by name or region"
          value={searchTerm}
          onChange={(event) => setSearchTerm(event.target.value)}
          className="artists-filters__input"
        />

        <select
          value={selectedTribe}
          onChange={(event) => setSelectedTribe(event.target.value)}
          className="artists-filters__select"
        >
          <option value="">All Tribes</option>
          {tribeOptions.map((tribe) => (
            <option key={tribe} value={tribe}>
              {tribe}
            </option>
          ))}
        </select>
      </div>

      <div className="artists-grid">
        {filteredArtists.map((artist) => (
          <div key={artist.id} className="artist-card">
            <h3 className="artist-card__title">{artist.fullName}</h3>
            <div className="artist-card__details">
              <p><span>Name:</span> {artist.fullName}</p>
              <p><span>Region:</span> {artist.region}</p>
              <p><span>Tribe:</span> {artist.tribeName}</p>
              <p><span>Birth Year:</span> {artist.birthYear ?? 'Unknown'}</p>
              <p><span>Death Year:</span> {artist.deathYear ?? 'N/A'}</p>
            </div>
            <p className="artist-card__bio">{artist.biography}</p>
          </div>
        ))}
      </div>
    </PageLayout>
  );
}

export default ArtistsPage;
