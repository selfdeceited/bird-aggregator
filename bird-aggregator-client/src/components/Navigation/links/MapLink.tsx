import { Map as MapIcon } from 'lucide-react'
import { NavLinkButton } from '../NavStyled'
import React from 'react'

export const MapLink: React.FC = () => (
	<NavLinkButton to="/map" title="Map">
		<MapIcon size={14}/> Map
	</NavLinkButton>
)
