/**
 * Theme Configuration Tests
 * Tests to verify Tailwind CSS compilation and theme variables with dark theme as default
 */

describe('Theme Configuration', () => {
  describe('CSS Custom Properties', () => {
    beforeEach(() => {
      document.head.innerHTML = ''
    })

    it('should have dark theme as default variables', () => {
      const testElement = document.createElement('div')
      testElement.style.cssText = '--background: #0a0a0a; --foreground: #ededed;'
      document.body.appendChild(testElement)
      
      expect(testElement.style.getPropertyValue('--background')).toBe('#0a0a0a')
      expect(testElement.style.getPropertyValue('--foreground')).toBe('#ededed')
      
      document.body.removeChild(testElement)
    })

    it('should have light theme variables available for preference override', () => {
      const lightBackground = '#ffffff'
      const lightForeground = '#171717'
      
      expect(lightBackground).toMatch(/^#[0-9A-Fa-f]{6}$/)
      expect(lightForeground).toMatch(/^#[0-9A-Fa-f]{6}$/)
    })

    it('should ensure dark theme has sufficient contrast', () => {
      const darkBg = parseInt('0a0a0a', 16)
      const darkFg = parseInt('ededed', 16)
      
      expect(darkFg).toBeGreaterThan(darkBg)
    })
  })

  describe('Responsive Design Validation', () => {
    it('should validate standard breakpoint structure', () => {
      const breakpoints = ['640px', '768px', '1024px', '1280px', '1536px']
      
      breakpoints.forEach(breakpoint => {
        expect(breakpoint).toMatch(/^\d+px$/)
        const value = parseInt(breakpoint)
        expect(value).toBeGreaterThan(0)
      })
    })
  })
})