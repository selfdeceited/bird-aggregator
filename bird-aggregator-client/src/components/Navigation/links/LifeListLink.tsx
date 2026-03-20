import { List } from 'lucide-react'
import { NavLinkButton } from '../NavStyled'
import React from 'react'

export const LifeListLink: React.FC = () => (
	<NavLinkButton to="/lifelist" title="Life List">
		<List size={14}/> Life List
	</NavLinkButton>
)
