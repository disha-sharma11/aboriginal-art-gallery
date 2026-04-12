import { useEffect, useMemo, useState } from 'react';
import { MapContainer, TileLayer, Marker, Popup } from 'react-leaflet';
import L from 'leaflet';
import PageLayout from '../Components/PageLayout';
import { getArtifacts, getTribes } from '../services/api';
import 'leaflet/dist/leaflet.css';
import './MapPage.css';

const tribeIcon = new L.Icon({
  iconUrl: 'https://raw.githubusercontent.com/pointhi/leaflet-color-markers/master/img/marker-icon-blue.png',
  shadowUrl: 'https://cdnjs.cloudflare.com/ajax/libs/leaflet/0.7.7/images/marker-shadow.png',
  iconSize: [25, 41],
  iconAnchor: [12, 41],
  popupAnchor: [1, -34],
  shadowSize: [41, 41],
});

const artifactIcon = new L.Icon({
  iconUrl: 'https://raw.githubusercontent.com/pointhi/leaflet-color-markers/master/img/marker-icon-red.png',
  shadowUrl: 'https://cdnjs.cloudflare.com/ajax/libs/leaflet/0.7.7/images/marker-shadow.png',
  iconSize: [25, 41],
  iconAnchor: [12, 41],
  popupAnchor: [1, -34],
  shadowSize: [41, 41],
});

delete L.Icon.Default.prototype._getIconUrl;
L.Icon.Default.mergeOptions({
  iconRetinaUrl: require('leaflet/dist/images/marker-icon-2x.png'),
  iconUrl: require('leaflet/dist/images/marker-icon.png'),
  shadowUrl: require('leaflet/dist/images/marker-shadow.png'),
});

function MapPage() {
  const [artifacts, setArtifacts] = useState([]);
  const [tribes, setTribes] = useState([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState('');

  useEffect(() => {
    async function loadMapData() {
      try {
        const [artifactData, tribeData] = await Promise.all([
          getArtifacts(),
          getTribes(),
        ]);
        setArtifacts(artifactData);
        setTribes(tribeData);
      } catch (err) {
        setError(err.message);
      } finally {
        setLoading(false);
      }
    }

    loadMapData();
  }, []);

  const tribeMarkers = useMemo(() => {
    return tribes.filter((tribe) => tribe.latitude && tribe.longitude);
  }, [tribes]);

  const artifactMarkers = useMemo(() => {
    return artifacts.filter((artifact) => artifact.latitude && artifact.longitude);
  }, [artifacts]);

  return (
    <PageLayout title="Map">
      <div className="map-page__intro">
        <p>
          This map shows the geographical origins linked to Aboriginal tribes and artifacts stored in
          the gallery system.
        </p>
      </div>

      {loading && <p>Loading map...</p>}
      {error && <p>{error}</p>}

      {!loading && !error && (
        <div className="map-page">
          <div className="map-page__sidebar">
            <div className="map-page__panel">
              <h2>Legend</h2>
              <p><span className="map-page__dot map-page__dot--tribe"></span> Tribe markers</p>
              <p><span className="map-page__dot map-page__dot--artifact"></span> Artifact markers</p>
            </div>

            <div className="map-page__panel">
              <h2>Summary</h2>
              <p><strong>Tribes on map:</strong> {tribeMarkers.length}</p>
              <p><strong>Artifacts on map:</strong> {artifactMarkers.length}</p>
            </div>

            <div className="map-page__panel">
              <h2>Visible Tribes</h2>
              {tribeMarkers.length === 0 && <p>No tribe locations available.</p>}
              {tribeMarkers.map((tribe) => (
                <p key={tribe.id}>{tribe.name}</p>
              ))}
            </div>
          </div>

          <div className="map-page__map-wrapper">
            <MapContainer center={[-25.2744, 133.7751]} zoom={4} className="map-page__map">
              <TileLayer
                attribution='&copy; OpenStreetMap contributors'
                url="https://{s}.tile.openstreetmap.org/{z}/{x}/{y}.png"
              />

              {tribeMarkers.map((tribe) => (
                <Marker
                  key={`tribe-${tribe.id}`}
                  position={[tribe.latitude, tribe.longitude]}
                  icon={tribeIcon}
                >
                  <Popup>
                    <strong>Tribe:</strong> {tribe.name}
                    <br />
                    <strong>Region:</strong> {tribe.originRegionName}
                    <br />
                    <strong>State/Territory:</strong> {tribe.stateOrTerritory}
                  </Popup>
                </Marker>
              ))}

              {artifactMarkers.map((artifact) => (
                <Marker
                  key={`artifact-${artifact.id}`}
                  position={[artifact.latitude, artifact.longitude]}
                  icon={artifactIcon}
                >
                  <Popup>
                    <strong>Artifact:</strong> {artifact.title}
                    <br />
                    <strong>Artist:</strong> {artifact.artistName}
                    <br />
                    <strong>Tribe:</strong> {artifact.tribeName}
                    <br />
                    <strong>Origin:</strong> {artifact.originPlaceName}
                  </Popup>
                </Marker>
              ))}
            </MapContainer>
          </div>
        </div>
      )}
    </PageLayout>
  );
}

export default MapPage;
