import { MemoryRouter } from 'react-router-dom';
import { render, screen, within } from '@testing-library/react';
import HomePage from './pages/HomePage';

function renderHomePage() {
  render(
    <MemoryRouter future={{ v7_startTransition: true, v7_relativeSplatPath: true }}>
      <HomePage />
    </MemoryRouter>
  );
}

describe('HomePage', () => {
  test('renders the page heading and description', () => {
    renderHomePage();

    expect(
      screen.getByRole('heading', { level: 1, name: 'Aboriginal Art Gallery' })
    ).toBeInTheDocument();

    expect(
      screen.getByText(
        /Explore Aboriginal tribes, artists, artifacts, and visitor comments in one gallery system/i
      )
    ).toBeInTheDocument();
  });

  test('renders the homepage feature cards', () => {
    renderHomePage();

    const artifactsCard = screen.getByRole('link', {
      name: /Artifacts Browse Aboriginal artworks and explore their details\./i,
    });

    const artistsCard = screen.getByRole('link', {
      name: /Artists View artist information and their cultural connections\./i,
    });

    const tribesCard = screen.getByRole('link', {
      name: /Tribes Learn about Aboriginal tribes and their origin regions\./i,
    });

    const commentsCard = screen.getByRole('link', {
      name: /Comments Read visitor feedback connected to gallery artifacts\./i,
    });

    expect(artifactsCard).toBeInTheDocument();
    expect(artistsCard).toBeInTheDocument();
    expect(tribesCard).toBeInTheDocument();
    expect(commentsCard).toBeInTheDocument();
  });

  test('shows all four navigation cards inside the homepage grid', () => {
    renderHomePage();

    const grid = screen.getByRole('heading', { level: 2, name: 'Artifacts' }).closest('div');
    const links = within(grid).getAllByRole('link');

    expect(links).toHaveLength(4);
  });
});
