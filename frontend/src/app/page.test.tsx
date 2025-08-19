import { render, screen } from '@testing-library/react'
import Home from './page'

describe('Home page', () => {
  it('renders the Todo App title', () => {
    render(<Home />)
    
    const heading = screen.getByRole('heading', { name: /todo app/i })
    expect(heading).toBeInTheDocument()
  })

  it('renders the boilerplate ready message', () => {
    render(<Home />)
    
    const message = screen.getByText(/frontend boilerplate ready for development/i)
    expect(message).toBeInTheDocument()
  })
})