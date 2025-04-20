import { useState } from 'react'
import './App.css'

function App() {
  const [count] = useState(0)

  return (
    <>
      <h1>The Arbitrary Counter Service</h1>
      <div className="card">
        <h3>Count: {count}</h3>
        <p>Test-change-and-merge</p>
      </div>
    </>
  )
}

export default App
