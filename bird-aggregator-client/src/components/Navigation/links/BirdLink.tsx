import { Link } from 'react-router-dom'
import React from 'react'

export const BirdLink: React.FC<{birdId: number}> = ({ birdId }) => <Link
	to={`/birds/${birdId}`}
	role="button"
	className="bp5-button bp5-minimal bp5-icon-arrow-right"
></Link>
