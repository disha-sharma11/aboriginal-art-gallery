import { MemoryRouter } from 'react-router-dom';
import { render, screen } from '@testing-library/react';
import CommentsPage from './CommentsPage';

describe('CommentsPage', () => {
  test('renders comments guidance for users', () => {
    render(
      <MemoryRouter future={{ v7_startTransition: true, v7_relativeSplatPath: true }}>
        <CommentsPage />
      </MemoryRouter>
    );

    expect(screen.getByRole('heading', { level: 1, name: 'Comments' })).toBeInTheDocument();
    expect(screen.getByRole('heading', { level: 2, name: 'Visitor Comments' })).toBeInTheDocument();
    expect(
      screen.getByText(/Comments are currently shown inside each artifact details page/i)
    ).toBeInTheDocument();
    expect(
      screen.getByText(/Open the Artifacts page, choose an artifact/i)
    ).toBeInTheDocument();
  });
});
