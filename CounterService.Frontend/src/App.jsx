import { useEffect, useState } from "react";
import "./App.css";
import { useFetch } from "./hooks/useFetch";

function App() {
	const [count] = useState(0);
	const { getData } = useFetch();

	useEffect(() => {
		getData("https://pokeapi.co/api/v2/pokemon/ditto")
			.then((response) => {
				if (response.ok) return response.json();
			})
			.then((data) => console.log(data));
	}, []);

	return (
		<>
			<h1>The Arbitrary Counter Service</h1>
			<div className="card">
				<h3>Count: {count}</h3>
				<button onClick={() => console.log("increase")}>Add + 1</button>
			</div>
		</>
	);
}

export default App;
