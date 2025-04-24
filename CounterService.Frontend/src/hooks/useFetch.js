export const useFetch = () => {
	const fetchData = (method) => (url, data) => fetch(url, {
		method: method,
		headers: {
			"Content-Type": "application/json"
		},
		body: data ? data : null,
	})

	const getData = fetchData("GET")

	return {
		getData
	}
}