import React from 'react'

export const ImageHostingLink: React.FC<{imageHostingLink: string}> = ({ imageHostingLink }) => <a
	role="button"
	className="bp5-button bp5-minimal bp5-icon-group-objects"
	href={imageHostingLink}
>
            Flickr
</a>
