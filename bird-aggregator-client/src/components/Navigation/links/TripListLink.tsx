import { Link } from 'react-router-dom'
import React from 'react'

export const TripListLink: React.FC = () => <Link
	to="/triplist"
	title="Trips"
	role="button"
	className="bp5-button bp5-minimal bp5-icon-torch small-margin"
>
    Trips
</Link>
