import { MemoryRouter } from 'react-router-dom';
import { render, screen, waitFor, within } from '@testing-library/react';
import ArtifactsPage from './ArtifactsPage';
import * as api from '../services/api';

jest.mock('../services/api');

describe('ArtifactsPage', () => {
  test('renders artifact cards after loading data', async () => {
    api.getArtifacts.mockResolvedValue([
      {
        id: 1,
        title: 'Morning Star Pole',
        artistName: 'Test Artist',
        tribeName: 'Yolngu',
        artType: 'Ceremonial',
        exhibitionName: 'Living Traditions',
        originPlaceName: 'Arnhem Land',
        imageUrl: '',
      },
    ]);

    render(
      <MemoryRouter future={{ v7_startTransition: true, v7_relativeSplatPath: true }}>
        <ArtifactsPage />
      </MemoryRouter>
    );

    expect(screen.getByText(/Loading artifacts/i)).toBeInTheDocument();

    await waitFor(() => {
      const heading = screen.getByRole('heading', {
        level: 3,
        name: 'Morning Star Pole',
      });

      expect(heading).toBeInTheDocument();

      // 👇 Scope everything inside the specific artifact card
      const card = heading.closest('a');

      expect(within(card).getByText(/Test Artist/i)).toBeInTheDocument();
      expect(within(card).getByText(/Yolngu/i)).toBeInTheDocument();
    });
  });

  test('shows an error message when artifact loading fails', async () => {
    api.getArtifacts.mockRejectedValue(new Error('Failed to fetch artifacts'));

    render(
      <MemoryRouter future={{ v7_startTransition: true, v7_relativeSplatPath: true }}>
        <ArtifactsPage />
      </MemoryRouter>
    );

    await waitFor(() => {
      expect(screen.getByText('Failed to fetch artifacts')).toBeInTheDocument();
    });
  });
});