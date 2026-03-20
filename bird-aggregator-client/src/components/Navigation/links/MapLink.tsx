import React from 'react'
import { Map } from 'lucide-react'
import { NavLinkButton } from '../NavStyled'

export const MapLink: React.FC = () => (
	<NavLinkButton to="/map" title="Map">
		<Map size={14}/> Map
	</NavLinkButton>
)
