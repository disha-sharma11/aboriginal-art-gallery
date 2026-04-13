const BASE_URL = process.env.REACT_APP_API_BASE_URL || 'http://localhost:5141/api';

export async function getArtifacts() {
  const response = await fetch(`${BASE_URL}/Artifacts`);
  if (!response.ok) throw new Error('Failed to fetch artifacts');
  return response.json();
}

export async function getArtifactById(id) {
  const response = await fetch(`${BASE_URL}/Artifacts/${id}`);
  if (!response.ok) throw new Error('Failed to fetch artifact');
  return response.json();
}

export async function getArtists() {
  const response = await fetch(`${BASE_URL}/Artists`);
  if (!response.ok) throw new Error('Failed to fetch artists');
  return response.json();
}

export async function getTribes() {
  const response = await fetch(`${BASE_URL}/Tribes`);
  if (!response.ok) throw new Error('Failed to fetch tribes');
  return response.json();
}

export async function getCommentsByArtifactId(id) {
  const response = await fetch(`${BASE_URL}/Comments/artifact/${id}`);
  if (!response.ok) throw new Error('Failed to fetch comments');
  return response.json();
}

export async function createComment(commentData) {
  const response = await fetch(`${BASE_URL}/Comments`, {
    method: 'POST',
    headers: {
      'Content-Type': 'application/json',
    },
    body: JSON.stringify(commentData),
  });

  if (!response.ok) throw new Error('Failed to create comment');
  return response.json();
}

export async function createTribe(tribeData) {
  const response = await fetch(`${BASE_URL}/Tribes`, {
    method: 'POST',
    headers: { 'Content-Type': 'application/json' },
    body: JSON.stringify(tribeData),
  });

  if (!response.ok) throw new Error('Failed to create tribe');
  return response.json();
}

export async function deleteTribe(id) {
  const response = await fetch(`${BASE_URL}/Tribes/${id}`, {
    method: 'DELETE',
  });

  if (!response.ok) throw new Error('Failed to delete tribe');
  return response.text();
}

export async function createArtist(artistData) {
  const response = await fetch(`${BASE_URL}/Artists`, {
    method: 'POST',
    headers: { 'Content-Type': 'application/json' },
    body: JSON.stringify(artistData),
  });

  if (!response.ok) throw new Error('Failed to create artist');
  return response.json();
}

export async function deleteArtist(id) {
  const response = await fetch(`${BASE_URL}/Artists/${id}`, {
    method: 'DELETE',
  });

  if (!response.ok) throw new Error('Failed to delete artist');
  return response.text();
}

export async function createArtifact(artifactData) {
  const response = await fetch(`${BASE_URL}/Artifacts`, {
    method: 'POST',
    headers: { 'Content-Type': 'application/json' },
    body: JSON.stringify(artifactData),
  });

  if (!response.ok) throw new Error('Failed to create artifact');
  return response.json();
}

export async function deleteArtifact(id) {
  const response = await fetch(`${BASE_URL}/Artifacts/${id}`, {
    method: 'DELETE',
  });

  if (!response.ok) throw new Error('Failed to delete artifact');
  return response.text();
}

export async function getExhibitions() {
  const response = await fetch(`${BASE_URL}/Exhibitions`);
  if (!response.ok) throw new Error('Failed to fetch exhibitions');
  return response.json();
}

export async function createExhibition(exhibitionData) {
  const response = await fetch(`${BASE_URL}/Exhibitions`, {
    method: 'POST',
    headers: { 'Content-Type': 'application/json' },
    body: JSON.stringify(exhibitionData),
  });

  if (!response.ok) throw new Error('Failed to create exhibition');
  return response.json();
}

export async function deleteExhibition(id) {
  const response = await fetch(`${BASE_URL}/Exhibitions/${id}`, {
    method: 'DELETE',
  });

  if (!response.ok) throw new Error('Failed to delete exhibition');
  return response.text();
}
