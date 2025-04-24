import { test, expect } from 'vitest'
import { render, screen } from '@testing-library/react'
import App from '../App'

test('should run test', () => {
	render(<App />)
	const title = screen.getByText(/Counter/)
	expect(title).not.toBeNull()
})