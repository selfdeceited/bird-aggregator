import { Link } from 'react-router-dom'
import React from 'react'

export const LatestPhotosLink: React.FC = () => (
	<Link to="/gallery" role="button" className="bp5-button bp5-minimal bp5-icon-camera small-margin">
            Check Out Full Gallery
	</Link>)
