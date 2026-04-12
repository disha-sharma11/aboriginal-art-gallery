import { useEffect, useState } from 'react';
import PageLayout from '../Components/PageLayout';
import {
  createTribe,
  createArtist,
  createArtifact,
  createExhibition,
  deleteTribe,
  deleteArtist,
  deleteArtifact,
  deleteExhibition,
  getArtifacts,
  getTribes,
  getArtists,
  getExhibitions,
} from '../services/api';
import './ManagePage.css';

function ManagePage() {
  const [tribes, setTribes] = useState([]);
  const [artists, setArtists] = useState([]);
  const [artifacts, setArtifacts] = useState([]);
  const [exhibitions, setExhibitions] = useState([]);
  const [message, setMessage] = useState('');

  const [tribeForm, setTribeForm] = useState({
    name: '',
    description: '',
    stateOrTerritory: '',
    originRegionName: '',
    latitude: '',
    longitude: '',
  });

  const [artistForm, setArtistForm] = useState({
    fullName: '',
    biography: '',
    birthYear: '',
    deathYear: '',
    region: '',
    photoUrl: '',
    aboriginalTribeId: '',
  });

  const [artifactForm, setArtifactForm] = useState({
    title: '',
    description: '',
    yearCreated: '',
    imageUrl: '',
    material: '',
    artType: '',
    artStyle: '',
    era: '',
    originPlaceName: '',
    latitude: '',
    longitude: '',
    artistId: '',
    aboriginalTribeId: '',
    exhibitionId: '',
  });

  const [exhibitionForm, setExhibitionForm] = useState({
    name: '',
    description: '',
    startDate: '',
    endDate: '',
    location: '',
  });

  async function loadData() {
    const [tribeData, artistData, artifactData, exhibitionData] = await Promise.all([
      getTribes(),
      getArtists(),
      getArtifacts(),
      getExhibitions(),
    ]);
    setTribes(tribeData);
    setArtists(artistData);
    setArtifacts(artifactData);
    setExhibitions(exhibitionData);
  }

  useEffect(() => {
    loadData();
  }, []);

  function handleTribeChange(event) {
    const { name, value } = event.target;
    setTribeForm((prev) => ({ ...prev, [name]: value }));
  }

  function handleArtistChange(event) {
    const { name, value } = event.target;
    setArtistForm((prev) => ({ ...prev, [name]: value }));
  }

  function handleArtifactChange(event) {
    const { name, value } = event.target;
    setArtifactForm((prev) => ({ ...prev, [name]: value }));
  }

  function handleExhibitionChange(event) {
    const { name, value } = event.target;
    setExhibitionForm((prev) => ({ ...prev, [name]: value }));
  }

  async function submitTribe(event) {
    event.preventDefault();
    try {
      await createTribe({
        ...tribeForm,
        latitude: tribeForm.latitude ? Number(tribeForm.latitude) : null,
        longitude: tribeForm.longitude ? Number(tribeForm.longitude) : null,
      });

      setMessage('Tribe created successfully.');
      setTribeForm({
        name: '',
        description: '',
        stateOrTerritory: '',
        originRegionName: '',
        latitude: '',
        longitude: '',
      });
      loadData();
    } catch {
      setMessage('Failed to create tribe.');
    }
  }

  async function submitArtist(event) {
    event.preventDefault();
    try {
      await createArtist({
        ...artistForm,
        birthYear: artistForm.birthYear ? Number(artistForm.birthYear) : null,
        deathYear: artistForm.deathYear ? Number(artistForm.deathYear) : null,
        aboriginalTribeId: Number(artistForm.aboriginalTribeId),
      });

      setMessage('Artist created successfully.');
      setArtistForm({
        fullName: '',
        biography: '',
        birthYear: '',
        deathYear: '',
        region: '',
        photoUrl: '',
        aboriginalTribeId: '',
      });
      loadData();
    } catch {
      setMessage('Failed to create artist.');
    }
  }

  async function submitArtifact(event) {
    event.preventDefault();
    try {
      await createArtifact({
        ...artifactForm,
        yearCreated: artifactForm.yearCreated ? Number(artifactForm.yearCreated) : null,
        latitude: artifactForm.latitude ? Number(artifactForm.latitude) : null,
        longitude: artifactForm.longitude ? Number(artifactForm.longitude) : null,
        artistId: Number(artifactForm.artistId),
        aboriginalTribeId: Number(artifactForm.aboriginalTribeId),
        exhibitionId: artifactForm.exhibitionId ? Number(artifactForm.exhibitionId) : null,
      });

      setMessage('Artifact created successfully.');
      setArtifactForm({
        title: '',
        description: '',
        yearCreated: '',
        imageUrl: '',
        material: '',
        artType: '',
        artStyle: '',
        era: '',
        originPlaceName: '',
        latitude: '',
        longitude: '',
        artistId: '',
        aboriginalTribeId: '',
        exhibitionId: '',
      });
      loadData();
    } catch {
      setMessage('Failed to create artifact.');
    }
  }

  async function submitExhibition(event) {
    event.preventDefault();
    try {
      await createExhibition({
        ...exhibitionForm,
        startDate: `${exhibitionForm.startDate}T00:00:00Z`,
        endDate: `${exhibitionForm.endDate}T00:00:00Z`,
      });

      setMessage('Exhibition created successfully.');
      setExhibitionForm({
        name: '',
        description: '',
        startDate: '',
        endDate: '',
        location: '',
      });
      loadData();
    } catch {
      setMessage('Failed to create exhibition.');
    }
  }

  async function handleDelete(type, id) {
    const shouldDelete = window.confirm('Delete this record?');

    if (!shouldDelete) {
      return;
    }

    try {
      if (type === 'tribe') {
        await deleteTribe(id);
      }

      if (type === 'artist') {
        await deleteArtist(id);
      }

      if (type === 'artifact') {
        await deleteArtifact(id);
      }

      if (type === 'exhibition') {
        await deleteExhibition(id);
      }

      setMessage('Record deleted successfully.');
      loadData();
    } catch {
      setMessage('Failed to delete record.');
    }
  }

  return (
    <PageLayout title="Manage Gallery">
      {message && <p className="manage-page__message">{message}</p>}

      <div className="manage-page__grid">
        <form onSubmit={submitTribe} className="manage-form">
          <h2>Create Tribe</h2>
          <input name="name" placeholder="Name" value={tribeForm.name} onChange={handleTribeChange} required />
          <textarea name="description" placeholder="Description" value={tribeForm.description} onChange={handleTribeChange} required />
          <input name="stateOrTerritory" placeholder="State/Territory" value={tribeForm.stateOrTerritory} onChange={handleTribeChange} required />
          <input name="originRegionName" placeholder="Origin Region" value={tribeForm.originRegionName} onChange={handleTribeChange} required />
          <input name="latitude" placeholder="Latitude" value={tribeForm.latitude} onChange={handleTribeChange} />
          <input name="longitude" placeholder="Longitude" value={tribeForm.longitude} onChange={handleTribeChange} />
          <button type="submit">Create Tribe</button>
        </form>

        <form onSubmit={submitArtist} className="manage-form">
          <h2>Create Artist</h2>
          <input name="fullName" placeholder="Full Name" value={artistForm.fullName} onChange={handleArtistChange} required />
          <textarea name="biography" placeholder="Biography" value={artistForm.biography} onChange={handleArtistChange} required />
          <input name="birthYear" placeholder="Birth Year" value={artistForm.birthYear} onChange={handleArtistChange} />
          <input name="deathYear" placeholder="Death Year" value={artistForm.deathYear} onChange={handleArtistChange} />
          <input name="region" placeholder="Region" value={artistForm.region} onChange={handleArtistChange} required />
          <input name="photoUrl" placeholder="Photo URL" value={artistForm.photoUrl} onChange={handleArtistChange} />
          <select name="aboriginalTribeId" value={artistForm.aboriginalTribeId} onChange={handleArtistChange} required>
            <option value="">Select Tribe</option>
            {tribes.map((tribe) => (
              <option key={tribe.id} value={tribe.id}>{tribe.name}</option>
            ))}
          </select>
          <button type="submit">Create Artist</button>
        </form>

        <form onSubmit={submitArtifact} className="manage-form">
          <h2>Create Artifact</h2>
          <input name="title" placeholder="Title" value={artifactForm.title} onChange={handleArtifactChange} required />
          <textarea name="description" placeholder="Description" value={artifactForm.description} onChange={handleArtifactChange} required />
          <input name="yearCreated" placeholder="Year Created" value={artifactForm.yearCreated} onChange={handleArtifactChange} />
          <input name="imageUrl" placeholder="Image URL" value={artifactForm.imageUrl} onChange={handleArtifactChange} />
          <input name="material" placeholder="Material" value={artifactForm.material} onChange={handleArtifactChange} required />
          <input name="artType" placeholder="Art Type" value={artifactForm.artType} onChange={handleArtifactChange} required />
          <input name="artStyle" placeholder="Art Style" value={artifactForm.artStyle} onChange={handleArtifactChange} required />
          <input name="era" placeholder="Era" value={artifactForm.era} onChange={handleArtifactChange} required />
          <input name="originPlaceName" placeholder="Origin Place" value={artifactForm.originPlaceName} onChange={handleArtifactChange} required />
          <input name="latitude" placeholder="Latitude" value={artifactForm.latitude} onChange={handleArtifactChange} />
          <input name="longitude" placeholder="Longitude" value={artifactForm.longitude} onChange={handleArtifactChange} />
          <select name="artistId" value={artifactForm.artistId} onChange={handleArtifactChange} required>
            <option value="">Select Artist</option>
            {artists.map((artist) => (
              <option key={artist.id} value={artist.id}>{artist.fullName}</option>
            ))}
          </select>
          <select name="aboriginalTribeId" value={artifactForm.aboriginalTribeId} onChange={handleArtifactChange} required>
            <option value="">Select Tribe</option>
            {tribes.map((tribe) => (
              <option key={tribe.id} value={tribe.id}>{tribe.name}</option>
            ))}
          </select>
          <select name="exhibitionId" value={artifactForm.exhibitionId} onChange={handleArtifactChange}>
            <option value="">No Exhibition</option>
            {exhibitions.map((exhibition) => (
              <option key={exhibition.id} value={exhibition.id}>{exhibition.name}</option>
            ))}
          </select>
          <button type="submit">Create Artifact</button>
        </form>

        <form onSubmit={submitExhibition} className="manage-form">
          <h2>Create Exhibition</h2>
          <input name="name" placeholder="Name" value={exhibitionForm.name} onChange={handleExhibitionChange} required />
          <textarea name="description" placeholder="Description" value={exhibitionForm.description} onChange={handleExhibitionChange} required />
          <input name="startDate" type="date" value={exhibitionForm.startDate} onChange={handleExhibitionChange} required />
          <input name="endDate" type="date" value={exhibitionForm.endDate} onChange={handleExhibitionChange} required />
          <input name="location" placeholder="Location" value={exhibitionForm.location} onChange={handleExhibitionChange} required />
          <button type="submit">Create Exhibition</button>
        </form>
      </div>

      <section className="manage-page__delete-section">
        <h2>Manager Records</h2>

        <div className="manage-page__delete-grid">
          <div className="manage-records">
            <h3>Tribes</h3>
            {tribes.map((tribe) => (
              <div key={tribe.id} className="manage-record">
                <span>{tribe.name}</span>
                <button type="button" onClick={() => handleDelete('tribe', tribe.id)}>Delete</button>
              </div>
            ))}
          </div>

          <div className="manage-records">
            <h3>Artists</h3>
            {artists.map((artist) => (
              <div key={artist.id} className="manage-record">
                <span>{artist.fullName}</span>
                <button type="button" onClick={() => handleDelete('artist', artist.id)}>Delete</button>
              </div>
            ))}
          </div>

          <div className="manage-records">
            <h3>Artifacts</h3>
            {artifacts.map((artifact) => (
              <div key={artifact.id} className="manage-record">
                <span>{artifact.title}</span>
                <button type="button" onClick={() => handleDelete('artifact', artifact.id)}>Delete</button>
              </div>
            ))}
          </div>

          <div className="manage-records">
            <h3>Exhibitions</h3>
            {exhibitions.map((exhibition) => (
              <div key={exhibition.id} className="manage-record">
                <span>{exhibition.name}</span>
                <button type="button" onClick={() => handleDelete('exhibition', exhibition.id)}>Delete</button>
              </div>
            ))}
          </div>
        </div>
      </section>
    </PageLayout>
  );
}

export default ManagePage;
