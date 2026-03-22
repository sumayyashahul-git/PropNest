import { useState, useEffect } from 'react'
import { useParams, useNavigate } from 'react-router-dom'
import { getProperty } from '../services/api'
import Layout from '../components/Layout'

export default function PropertyDetailPage() {
  const { id } = useParams()
  const navigate = useNavigate()
  const [property, setProperty] = useState(null)
  const [loading, setLoading] = useState(true)

  useEffect(() => {
    fetchProperty()
  }, [id])

  const fetchProperty = async () => {
    try {
      const response = await getProperty(id)
      setProperty(response.data)
    } catch (err) {
      navigate('/properties')
    } finally {
      setLoading(false)
    }
  }

  const formatPrice = (price) => {
    return new Intl.NumberFormat('en-AE', {
      style: 'currency',
      currency: 'AED',
      maximumFractionDigits: 0
    }).format(price)
  }

  const propertyTypes = ['Apartment', 'Villa', 'Townhouse', 'Penthouse', 'Studio', 'Office']

  if (loading) {
    return (
      <Layout>
        <div className="text-center py-20 text-slate-400">
          Loading property...
        </div>
      </Layout>
    )
  }

  if (!property) return null

  return (
    <Layout>
      <div className="max-w-5xl mx-auto">

        {/* Back button */}
        <button
          onClick={() => navigate('/properties')}
          className="flex items-center gap-2 text-slate-500 hover:text-slate-700 mb-6 text-sm transition-colors">
          ← Back to Properties
        </button>

        <div className="bg-white rounded-2xl shadow-sm overflow-hidden">

          {/* Image */}
          <div className="h-72 bg-gradient-to-br from-slate-200 to-slate-300 flex items-center justify-center">
            <span className="text-8xl">🏠</span>
          </div>

          <div className="p-8">

            {/* Badges */}
            <div className="flex gap-2 mb-4">
              <span className="bg-red-50 text-red-600 text-xs font-medium px-3 py-1 rounded-full">
                {propertyTypes[property.type]}
              </span>
              <span className="bg-blue-50 text-blue-600 text-xs font-medium px-3 py-1 rounded-full">
                {property.listingType === 0 ? 'For Sale' : 'For Rent'}
              </span>
              <span className={`text-xs font-medium px-3 py-1 rounded-full ${
                property.status === 0
                  ? 'bg-green-50 text-green-600'
                  : 'bg-yellow-50 text-yellow-600'
              }`}>
                {property.status === 0 ? 'Available' : 'Under Offer'}
              </span>
            </div>

            {/* Title and price */}
            <div className="flex justify-between items-start mb-4">
              <h1 className="text-2xl font-bold text-slate-800 flex-1">
                {property.title}
              </h1>
              <div className="text-right ml-6">
                <div className="text-2xl font-bold text-red-500">
                  {formatPrice(property.price)}
                </div>
                {property.serviceCharge > 0 && (
                  <div className="text-sm text-slate-400 mt-1">
                    Service charge: {formatPrice(property.serviceCharge)}/year
                  </div>
                )}
              </div>
            </div>

            {/* Location */}
            <p className="text-slate-500 mb-6">
              📍 {property.community}, {property.location}, {property.emirate}
            </p>

            {/* Stats grid */}
            <div className="grid grid-cols-3 gap-4 mb-6">
              <div className="bg-gray-50 rounded-xl p-4 text-center">
                <div className="text-2xl mb-1">🛏</div>
                <div className="text-lg font-semibold text-slate-800">{property.bedrooms}</div>
                <div className="text-xs text-slate-400">Bedrooms</div>
              </div>
              <div className="bg-gray-50 rounded-xl p-4 text-center">
                <div className="text-2xl mb-1">🚿</div>
                <div className="text-lg font-semibold text-slate-800">{property.bathrooms}</div>
                <div className="text-xs text-slate-400">Bathrooms</div>
              </div>
              <div className="bg-gray-50 rounded-xl p-4 text-center">
                <div className="text-2xl mb-1">📐</div>
                <div className="text-lg font-semibold text-slate-800">{property.areaSqFt}</div>
                <div className="text-xs text-slate-400">Sq Ft</div>
              </div>
            </div>

            {/* Amenities */}
            <div className="mb-6">
              <h3 className="text-sm font-semibold text-slate-700 mb-3">Amenities</h3>
              <div className="flex flex-wrap gap-2">
                {property.hasParking && (
                  <span className="bg-slate-100 text-slate-600 text-xs px-3 py-1 rounded-full">
                    🚗 Parking
                  </span>
                )}
                {property.hasPool && (
                  <span className="bg-slate-100 text-slate-600 text-xs px-3 py-1 rounded-full">
                    🏊 Pool
                  </span>
                )}
                {property.hasGym && (
                  <span className="bg-slate-100 text-slate-600 text-xs px-3 py-1 rounded-full">
                    💪 Gym
                  </span>
                )}
                {property.isFurnished && (
                  <span className="bg-slate-100 text-slate-600 text-xs px-3 py-1 rounded-full">
                    🛋 Furnished
                  </span>
                )}
              </div>
            </div>

            {/* Description */}
            {property.description && (
              <div className="mb-6">
                <h3 className="text-sm font-semibold text-slate-700 mb-3">Description</h3>
                <p className="text-slate-500 text-sm leading-relaxed">
                  {property.description}
                </p>
              </div>
            )}

            {/* Agent info */}
            <div className="border-t border-gray-100 pt-6 flex justify-between items-center">
              <div>
                <p className="text-xs text-slate-400 mb-1">Listed by</p>
                <p className="text-sm font-semibold text-slate-700">
                  👤 {property.agentName}
                </p>
                <p className="text-xs text-slate-400 mt-1">
                  👁 {property.viewCount} views
                </p>
              </div>
              <button className="bg-red-500 hover:bg-red-600 text-white px-6 py-3 rounded-xl font-semibold text-sm transition-colors">
                Contact Agent
              </button>
            </div>

          </div>
        </div>
      </div>
    </Layout>
  )
}