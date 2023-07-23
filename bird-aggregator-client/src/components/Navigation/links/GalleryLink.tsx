import { Link } from 'react-router-dom'
import React from 'react'

export const GalleryLink: React.FC = () => <Link
	to={'/'}
	role="button"
	className="bp5-button bp5-minimal bp5-icon-camera"
>
    Gallery
</Link>
