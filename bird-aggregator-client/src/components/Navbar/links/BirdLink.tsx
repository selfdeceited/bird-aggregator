import { Link } from 'react-router-dom'
import React from 'react'

export const BirdLink: React.FC<{birdId: number}> = ({ birdId }) => <Link
	to={`/birds/${birdId}`}
	role="button"
	className="bp3-button bp3-minimal bp3-icon-arrow-right"
></Link>
