import { Link } from 'react-router-dom'
import styled from 'styled-components'

export const NavBar = styled.nav`
  position: fixed;
  top: 0;
  left: 0;
  right: 0;
  height: 50px;
  display: flex;
  align-items: center;
  padding: 0 10px;
  background: #f6f7f9;
  box-shadow: 0 1px 3px rgba(0, 0, 0, 0.2);
  z-index: 10;
`

type NavGroupProps = { $align?: 'left' | 'right' }

export const NavGroup = styled.div<NavGroupProps>`
  display: flex;
  align-items: center;
  flex: ${(p: NavGroupProps) => p.$align === 'right' ? '0 0 auto' : '1'};
  gap: 2px;
`

export const NavDivider = styled.div`
  width: 1px;
  height: 20px;
  background: #d0d0d0;
  margin: 0 8px;
`

export const NavHeading = styled(Link)`
  font-weight: 600;
  font-size: 14px;
  text-decoration: none;
  color: #182026;
  padding: 0 8px;
  &:hover { text-decoration: underline; }
`

export const NavLinkButton = styled(Link)`
  display: inline-flex;
  align-items: center;
  gap: 5px;
  padding: 5px 10px;
  border-radius: 3px;
  text-decoration: none;
  color: #182026;
  font-size: 14px;
  background: transparent;
  border: none;
  cursor: pointer;
  &:hover { background: rgba(167, 182, 194, 0.3); }
`

export const NavAButton = styled.a`
  display: inline-flex;
  align-items: center;
  gap: 5px;
  padding: 5px 10px;
  border-radius: 3px;
  text-decoration: none;
  color: #182026;
  font-size: 14px;
  background: transparent;
  border: none;
  cursor: pointer;
  &:hover { background: rgba(167, 182, 194, 0.3); }
`

export const NavButton = styled.button`
  display: inline-flex;
  align-items: center;
  gap: 5px;
  padding: 5px 10px;
  border-radius: 3px;
  color: #182026;
  font-size: 14px;
  background: transparent;
  border: 1px solid transparent;
  cursor: pointer;
  &:hover { background: rgba(167, 182, 194, 0.3); }
`
