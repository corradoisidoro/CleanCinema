import { defineConfig } from 'vite'
import react from '@vitejs/plugin-react-swc'

// https://vite.dev/config/
export default defineConfig({
  plugins: [react()],
  server: {
    // Vite otherwise binds the IPv6 loopback only, so the dev server is
    // unreachable at http://127.0.0.1:5173 and from other machines on
    // the network. Binding all interfaces makes localhost, 127.0.0.1 and
    // the LAN address all resolve to the same server.
    host: true,
    // Fail loudly instead of silently falling back to 5174, which is a
    // common source of "the page is blank" confusion.
    strictPort: true,
  },
})
