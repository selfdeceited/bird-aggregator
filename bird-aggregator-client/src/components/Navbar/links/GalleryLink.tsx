import { Link } from 'react-router-dom'
import React from 'react'

export const GalleryLink: React.FC = () => <Link
	to={'/'}
	role="button"
	className="bp3-button bp3-minimal bp3-icon-camera"
>
    Gallery
</Link>
