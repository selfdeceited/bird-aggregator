import { Link } from 'react-router-dom'
import React from 'react'

export const LatestPhotosLink: React.FC = () => (
	<div>
		<h2 className="latest-shots">Latest photos</h2>
		<Link to="/gallery" role="button" className="bp3-button bp3-minimal bp3-icon-camera small-margin">
            Check Out Full Gallery
		</Link>
	</div>)
