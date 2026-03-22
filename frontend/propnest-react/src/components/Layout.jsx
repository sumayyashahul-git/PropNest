import { Link, useNavigate } from 'react-router-dom'

export default function Layout({ children }) {
  const navigate = useNavigate()
  const token = localStorage.getItem('token')
  const userName = localStorage.getItem('userName')
  const userRole = localStorage.getItem('userRole')

  const handleLogout = () => {
    localStorage.removeItem('token')
    localStorage.removeItem('userName')
    localStorage.removeItem('userRole')
    navigate('/login')
  }

  return (
    <div className="min-h-screen flex flex-col bg-gray-50">

      {/* ── HEADER ── */}
      <header className="bg-slate-900 px-10 h-16 flex items-center justify-between shadow-lg">

        {/* Logo */}
        <Link to="/" className="text-red-400 text-xl font-bold tracking-wide">
          🏠 PropNest
        </Link>

        {/* Nav */}
        <nav className="flex items-center gap-6">
          <Link to="/" className="text-slate-300 hover:text-white text-sm font-medium transition-colors">
            Home
          </Link>
          <Link to="/properties" className="text-slate-300 hover:text-white text-sm font-medium transition-colors">
            Properties
          </Link>

          {!token ? (
            <>
              <Link to="/login" className="text-slate-300 hover:text-white text-sm font-medium transition-colors">
                Login
              </Link>
              <Link to="/register" className="bg-red-500 hover:bg-red-600 text-white text-sm font-medium px-4 py-2 rounded-lg transition-colors">
                Register
              </Link>
            </>
          ) : (
            <>
              <span className="text-slate-400 text-sm">
                👤 {userName} ({userRole})
              </span>
              <button
                onClick={handleLogout}
                className="border border-red-500 text-red-400 hover:bg-red-500 hover:text-white text-sm px-4 py-2 rounded-lg transition-colors"
              >
                Logout
              </button>
            </>
          )}
        </nav>
      </header>

      {/* ── MAIN ── */}
      <main className="flex-1 px-10 py-8">
        {children}
      </main>

      {/* ── FOOTER ── */}
      <footer className="bg-slate-900 text-slate-400 px-10 py-5 flex justify-between items-center text-sm">
        <span>© 2024 PropNest — UAE Real Estate Platform</span>
        <div className="flex gap-6">
          <Link to="/properties" className="text-slate-400 hover:text-white transition-colors">
            Properties
          </Link>
          <Link to="/login" className="text-slate-400 hover:text-white transition-colors">
            Agent Login
          </Link>
        </div>
      </footer>

    </div>
  )
}
