import { Link } from 'react-router-dom';
import './Navbar.css';

function Navbar() {
  return (
    <nav className="navbar">
      <div className="navbar-logo">
        <Link to="/">Aboriginal Art Gallery</Link>
      </div>

      <div className="navbar-links">
        <Link to="/">Home</Link>
        <Link to="/artifacts">Artifacts</Link>
        <Link to="/artists">Artists</Link>
        <Link to="/tribes">Tribes</Link>
        <Link to="/exhibitions">Exhibitions</Link>
        <Link to="/comments">Comments</Link>
        <Link to="/map">Map</Link>
        <Link to="/manage">Manage Gallery</Link>
      </div>
    </nav>
  );
}

export default Navbar;
