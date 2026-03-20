import { ArrowRight } from 'lucide-react'
import { NavLinkButton } from '../NavStyled'
import React from 'react'

export const BirdLink: React.FC<{ birdId: number }> = ({ birdId }) => (
	<NavLinkButton to={`/birds/${birdId}`} title="View bird">
		<ArrowRight size={14}/>
	</NavLinkButton>
)
