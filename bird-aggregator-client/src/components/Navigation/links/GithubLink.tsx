import React from 'react'

export const GithubLink: React.FC<{githubLink: string}> = ({ githubLink }) => <a
	role="button"
	className="bp5-button bp5-minimal bp5-icon-git-repo"
	href={githubLink}
>
            GitHub
</a>
