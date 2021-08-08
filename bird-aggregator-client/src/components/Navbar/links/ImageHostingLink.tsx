import React from 'react'

export const ImageHostingLink: React.FC<{imageHostingLink: string}> = ({ imageHostingLink }) => <a
	role="button"
	className="bp3-button bp3-minimal bp3-icon-group-objects"
	href={imageHostingLink}
>
            Flickr
</a>
