import React from 'react'
import { ArrowRight } from 'lucide-react'
import { NavLinkButton } from '../NavStyled'

export const BirdLink: React.FC<{ birdId: number }> = ({ birdId }) => (
	<NavLinkButton to={`/birds/${birdId}`} title="View bird">
		<ArrowRight size={14}/>
	</NavLinkButton>
)
