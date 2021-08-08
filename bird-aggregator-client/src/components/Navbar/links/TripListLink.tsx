import { Link } from 'react-router-dom'
import React from 'react'

export const TripListLink: React.FC = () => <Link
	to="/triplist"
	title="Trips"
	role="button"
	className="bp3-button bp3-minimal bp3-icon-torch small-margin"
>
    Trips
</Link>
