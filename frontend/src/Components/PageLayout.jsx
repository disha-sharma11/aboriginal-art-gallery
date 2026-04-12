import Navbar from './Navbar';
import Footer from './Footer';
import './PageLayout.css';

function PageLayout({ title, children }) {
  return (
    <div className="page-layout">
      <Navbar />
      <main className="page-layout__content">
        {title ? <h1 className="page-layout__title">{title}</h1> : null}
        {children}
      </main>
      <Footer />
    </div>
  );
}

export default PageLayout;
