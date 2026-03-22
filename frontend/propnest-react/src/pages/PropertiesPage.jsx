import { useState, useEffect } from 'react'
import { Link } from 'react-router-dom'
import { searchProperties } from '../services/api'
import Layout from '../components/Layout'

export default function PropertiesPage() {
  const [properties, setProperties] = useState([])
  const [loading, setLoading] = useState(true)
  const [total, setTotal] = useState(0)
  const [page, setPage] = useState(1)
  const [filters, setFilters] = useState({
    location: '',
    type: '',
    listingType: '',
    minPrice: '',
    maxPrice: '',
    minBedrooms: ''
  })

  useEffect(() => {
    fetchProperties()
  }, [page])

  const fetchProperties = async () => {
    setLoading(true)
    try {
      const params = { page }
      if (filters.location)    params.location = filters.location
      if (filters.type !== '')        params.type = filters.type
      if (filters.listingType !== '') params.listingType = filters.listingType
      if (filters.minPrice)   params.minPrice = filters.minPrice
      if (filters.maxPrice)   params.maxPrice = filters.maxPrice
      if (filters.minBedrooms) params.minBedrooms = filters.minBedrooms

      const response = await searchProperties(params)
      setProperties(response.data.items)
      setTotal(response.data.total)
    } catch (err) {
      console.error('Failed to fetch properties', err)
    } finally {
      setLoading(false)
    }
  }

  const handleSearch = (e) => {
    e.preventDefault()
    setPage(1)
    fetchProperties()
  }

  const handleFilterChange = (e) => {
    setFilters({ ...filters, [e.target.name]: e.target.value })
  }

  const formatPrice = (price) => {
    return new Intl.NumberFormat('en-AE', {
      style: 'currency',
      currency: 'AED',
      maximumFractionDigits: 0
    }).format(price)
  }

  const propertyTypes = ['Apartment', 'Villa', 'Townhouse', 'Penthouse', 'Studio', 'Office']

  return (
    <Layout>
      <div className="max-w-7xl mx-auto">

        {/* ── PAGE TITLE ── */}
        <div className="mb-6">
          <h1 className="text-3xl font-bold text-slate-800">Properties</h1>
          <p className="text-slate-500 mt-1">{total} properties found</p>
        </div>

        {/* ── SEARCH FILTERS ── */}
        <form onSubmit={handleSearch}
          className="bg-white rounded-2xl shadow-sm p-6 mb-8 grid grid-cols-2 md:grid-cols-3 lg:grid-cols-6 gap-4">

          <input
            name="location"
            value={filters.location}
            onChange={handleFilterChange}
            placeholder="Location..."
            className="border border-gray-200 rounded-lg px-3 py-2 text-sm focus:outline-none focus:ring-2 focus:ring-red-400"
          />

          <select
            name="type"
            value={filters.type}
            onChange={handleFilterChange}
            className="border border-gray-200 rounded-lg px-3 py-2 text-sm focus:outline-none focus:ring-2 focus:ring-red-400 bg-white"
          >
            <option value="">All Types</option>
            {propertyTypes.map((t, i) => (
              <option key={t} value={i}>{t}</option>
            ))}
          </select>

          <select
            name="listingType"
            value={filters.listingType}
            onChange={handleFilterChange}
            className="border border-gray-200 rounded-lg px-3 py-2 text-sm focus:outline-none focus:ring-2 focus:ring-red-400 bg-white"
          >
            <option value="">Sale & Rent</option>
            <option value="0">For Sale</option>
            <option value="1">For Rent</option>
          </select>

          <input
            name="minPrice"
            value={filters.minPrice}
            onChange={handleFilterChange}
            placeholder="Min Price"
            type="number"
            className="border border-gray-200 rounded-lg px-3 py-2 text-sm focus:outline-none focus:ring-2 focus:ring-red-400"
          />

          <input
            name="maxPrice"
            value={filters.maxPrice}
            onChange={handleFilterChange}
            placeholder="Max Price"
            type="number"
            className="border border-gray-200 rounded-lg px-3 py-2 text-sm focus:outline-none focus:ring-2 focus:ring-red-400"
          />

          <button type="submit"
            className="bg-red-500 hover:bg-red-600 text-white rounded-lg px-4 py-2 text-sm font-semibold transition-colors">
            Search
          </button>

        </form>

        {/* ── LOADING ── */}
        {loading && (
          <div className="text-center py-20 text-slate-400">
            Loading properties...
          </div>
        )}

        {/* ── NO RESULTS ── */}
        {!loading && properties.length === 0 && (
          <div className="text-center py-20">
            <p className="text-slate-400 text-lg">No properties found.</p>
            <p className="text-slate-300 text-sm mt-2">Try adjusting your filters.</p>
          </div>
        )}

        {/* ── PROPERTY CARDS ── */}
        {!loading && properties.length > 0 && (
          <div className="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-3 gap-6">
            {properties.map(property => (
              <div key={property.id}
                className="bg-white rounded-2xl shadow-sm overflow-hidden hover:shadow-md transition-shadow">

                {/* Image placeholder */}
                <div className="h-48 bg-gradient-to-br from-slate-200 to-slate-300 flex items-center justify-center">
                  <span className="text-slate-400 text-4xl">🏠</span>
                </div>

                {/* Card content */}
                <div className="p-5">

                  {/* Type + Listing badges */}
                  <div className="flex gap-2 mb-3">
                    <span className="bg-red-50 text-red-600 text-xs font-medium px-2 py-1 rounded-full">
                      {propertyTypes[property.type]}
                    </span>
                    <span className="bg-blue-50 text-blue-600 text-xs font-medium px-2 py-1 rounded-full">
                      {property.listingType === 0 ? 'For Sale' : 'For Rent'}
                    </span>
                  </div>

                  {/* Title */}
                  <h3 className="font-semibold text-slate-800 text-base mb-1 line-clamp-1">
                    {property.title}
                  </h3>

                  {/* Location */}
                  <p className="text-slate-400 text-sm mb-3">
                    📍 {property.community}, {property.emirate}
                  </p>

                  {/* Details */}
                  <div className="flex gap-4 text-sm text-slate-500 mb-4">
                    <span>🛏 {property.bedrooms} Beds</span>
                    <span>🚿 {property.bathrooms} Baths</span>
                    <span>📐 {property.areaSqFt} sqft</span>
                  </div>

                  {/* Price + Button */}
                  <div className="flex items-center justify-between">
                    <span className="text-red-500 font-bold text-lg">
                      {formatPrice(property.price)}
                    </span>
                    <Link to={`/properties/${property.id}`}
                      className="bg-slate-800 hover:bg-slate-700 text-white text-xs px-4 py-2 rounded-lg transition-colors">
                      View Details
                    </Link>
                  </div>

                </div>
              </div>
            ))}
          </div>
        )}

        {/* ── PAGINATION ── */}
        {total > 12 && (
          <div className="flex justify-center gap-3 mt-10">
            <button
              onClick={() => setPage(p => Math.max(1, p - 1))}
              disabled={page === 1}
              className="px-4 py-2 rounded-lg border border-gray-200 text-sm disabled:opacity-40 hover:bg-gray-50">
              Previous
            </button>
            <span className="px-4 py-2 text-sm text-slate-500">
              Page {page}
            </span>
            <button
              onClick={() => setPage(p => p + 1)}
              disabled={properties.length < 12}
              className="px-4 py-2 rounded-lg border border-gray-200 text-sm disabled:opacity-40 hover:bg-gray-50">
              Next
            </button>
          </div>
        )}

      </div>
    </Layout>
  )
}