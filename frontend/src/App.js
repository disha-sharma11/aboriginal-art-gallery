import { Routes, Route } from 'react-router-dom';
import HomePage from './pages/HomePage';
import ArtifactsPage from './pages/ArtifactsPage';
import ArtifactDetailsPage from './pages/ArtifactDetailsPage';
import ArtistsPage from './pages/ArtistsPage';
import TribesPage from './pages/TribesPage';
import ExhibitionsPage from './pages/ExhibitionsPage';
import CommentsPage from './pages/CommentsPage';
import MapPage from './pages/MapPage';
import ManagePage from './pages/ManagePage';



function App() {
  return (
    <Routes>
      <Route path="/" element={<HomePage />} />
      <Route path="/artifacts" element={<ArtifactsPage />} />
      <Route path="/artifacts/:id" element={<ArtifactDetailsPage />} />
      <Route path="/artists" element={<ArtistsPage />} />
      <Route path="/tribes" element={<TribesPage />} />
      <Route path="/exhibitions" element={<ExhibitionsPage />} />
      <Route path="/comments" element={<CommentsPage />} />
      <Route path="/map" element={<MapPage />} />
      <Route path="/manage" element={<ManagePage />} />
    </Routes>
  );
}

export default App;
