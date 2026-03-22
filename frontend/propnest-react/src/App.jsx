import { Routes, Route, Navigate } from 'react-router-dom'
import Layout from './components/Layout'
import LoginPage from './pages/LoginPage'
import RegisterPage from './pages/RegisterPage'
import PropertiesPage from './pages/PropertiesPage'
import PropertyDetailPage from './pages/PropertyDetailPage'
import ProtectedRoute from './components/ProtectedRoute'

export default function App() {
  return (
    <Routes>
      <Route path="/" element={
        <Layout>
          <div className="text-center py-20">
            <h1 className="text-4xl font-bold text-slate-800 mb-4">
              Find Your Dream Property in UAE
            </h1>
            <p className="text-slate-500 text-lg mb-8">
              Browse thousands of properties across Dubai, Abu Dhabi and beyond
            </p>
            <a href="/properties"
              className="bg-red-500 hover:bg-red-600 text-white px-8 py-3 rounded-lg font-semibold transition-colors">
              Browse Properties
            </a>
          </div>
        </Layout>
      } />

      <Route path="/login" element={<LoginPage />} />
      <Route path="/register" element={<RegisterPage />} />

      <Route path="/properties" element={
        <ProtectedRoute>
          <PropertiesPage />
        </ProtectedRoute>
      } />

      <Route path="/properties/:id" element={
        <ProtectedRoute>
          <PropertyDetailPage />
        </ProtectedRoute>
      } />

      <Route path="*" element={<Navigate to="/" />} />
    </Routes>
  )
}
