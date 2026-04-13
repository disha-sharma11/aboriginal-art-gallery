import { MemoryRouter } from 'react-router-dom';
import { render, screen, within } from '@testing-library/react';
import HomePage from './pages/HomePage';

describe('HomePage', () => {
  test('renders the page heading and description', () => {
    render(
      <MemoryRouter>
        <HomePage />
      </MemoryRouter>
    );

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
    render(
      <MemoryRouter>
        <HomePage />
      </MemoryRouter>
    );

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
});
