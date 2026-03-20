import React from 'react'
import { Flame } from 'lucide-react'
import { NavLinkButton } from '../NavStyled'

export const TripListLink: React.FC = () => (
	<NavLinkButton to="/triplist" title="Trips">
		<Flame size={14}/> Trips
	</NavLinkButton>
)
