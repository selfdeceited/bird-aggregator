import React from 'react'
import { List } from 'lucide-react'
import { NavLinkButton } from '../NavStyled'

export const LifeListLink: React.FC = () => (
	<NavLinkButton to="/lifelist" title="Life List">
		<List size={14}/> Life List
	</NavLinkButton>
)
