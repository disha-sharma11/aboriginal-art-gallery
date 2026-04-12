import { Link } from 'react-router-dom';
import PageLayout from '../Components/PageLayout';
import './HomePage.css';

function HomePage() {
  return (
    <PageLayout title="Aboriginal Art Gallery">
      <p className="home-page__description">
        Explore Aboriginal tribes, artists, artifacts, and visitor comments in one gallery system.
      </p>

      <section className="home-page__section">
        <div className="home-page__grid">
          <Link to="/artifacts" className="home-page__card">
            <h2>Artifacts</h2>
            <p>Browse Aboriginal artworks and explore their details.</p>
          </Link>

          <Link to="/artists" className="home-page__card">
            <h2>Artists</h2>
            <p>View artist information and their cultural connections.</p>
          </Link>

          <Link to="/tribes" className="home-page__card">
            <h2>Tribes</h2>
            <p>Learn about Aboriginal tribes and their origin regions.</p>
          </Link>

          <Link to="/comments" className="home-page__card">
            <h2>Comments</h2>
            <p>Read visitor feedback connected to gallery artifacts.</p>
          </Link>
        </div>
      </section>
    </PageLayout>
  );
}

export default HomePage;
