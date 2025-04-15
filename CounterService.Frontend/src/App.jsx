import { useState } from 'react'
import './App.css' 

function App() {
  const [count, setCount] = useState(0)

  return (
    <>
      <h1>The Arbitrary Counter Service</h1>
      <div className="card">
        <h3>Count: {count}</h3>
      </div>
    </>
  )
}

export default App
