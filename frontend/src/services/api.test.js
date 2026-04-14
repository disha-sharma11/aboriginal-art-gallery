const originalFetch = global.fetch;
const originalBaseUrl = process.env.REACT_APP_API_BASE_URL;

describe('api service', () => {
  beforeEach(() => {
    jest.resetModules();
    global.fetch = jest.fn();
  });

  afterEach(() => {
    global.fetch = originalFetch;
    process.env.REACT_APP_API_BASE_URL = originalBaseUrl;
  });

  test('uses the configured API base URL for artifact requests', async () => {
    process.env.REACT_APP_API_BASE_URL = 'http://localhost:5142/api';
    global.fetch.mockResolvedValue({
      ok: true,
      json: async () => [{ id: 1, title: 'Artifact' }],
    });

    const { getArtifacts } = require('./api');
    await getArtifacts();

    expect(global.fetch).toHaveBeenCalledWith('http://localhost:5142/api/Artifacts');
  });

  test('falls back to the local default API base URL when no environment variable is set', async () => {
    delete process.env.REACT_APP_API_BASE_URL;
    global.fetch.mockResolvedValue({
      ok: true,
      json: async () => ({ id: 1 }),
    });

    const { getArtifactById } = require('./api');
    await getArtifactById(1);

    expect(global.fetch).toHaveBeenCalledWith('http://localhost:5141/api/Artifacts/1');
  });
});
