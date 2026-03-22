import axios from 'axios'

const AUTH_URL = 'http://localhost:5002/api'
const PROPERTY_URL = 'http://localhost:5001/api'
const ANALYTICS_URL = 'http://localhost:5003/api'

// ── helper: attach JWT token to every request ──
const authHeader = () => ({
  headers: {
    Authorization: `Bearer ${localStorage.getItem('token')}`
  }
})

// ── AUTH ──────────────────────────────────────
export const register = (data) =>
  axios.post(`${AUTH_URL}/Auth/register`, data)

export const login = (data) =>
  axios.post(`${AUTH_URL}/Auth/login`, data)

// ── PROPERTIES ────────────────────────────────
export const searchProperties = (params) =>
  axios.get(`${PROPERTY_URL}/Properties`, { params })

export const getProperty = (id) =>
  axios.get(`${PROPERTY_URL}/Properties/${id}`)

export const createProperty = (data) =>
  axios.post(`${PROPERTY_URL}/Properties`, data, authHeader())

export const updateProperty = (id, data) =>
  axios.put(`${PROPERTY_URL}/Properties/${id}`, data, authHeader())

export const deleteProperty = (id) =>
  axios.delete(`${PROPERTY_URL}/Properties/${id}`, authHeader())

// ── ANALYTICS ─────────────────────────────────
export const getDashboardStats = () =>
  axios.get(`${ANALYTICS_URL}/Analytics/dashboard`, authHeader())

export const getTopViewed = () =>
  axios.get(`${ANALYTICS_URL}/Analytics/top-viewed`, authHeader())